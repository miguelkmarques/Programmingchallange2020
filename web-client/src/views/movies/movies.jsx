import React from "react";
import {
  Row,
  Col,
  Card,
  CardHeader,
  CardBody,
  Form,
  FormGroup,
} from "reactstrap";
import BootstrapTable from "react-bootstrap-table-next";
import buildQuery from "odata-query";
import Joi from "joi-browser";
import formatNumber from "./../../helpers/formatNumber";
import moviesService from "./../../services/moviesService";
import genresService from "./../../services/genresService";
import DefaultForm from "./../../components/defaultForm";

class Movies extends DefaultForm {
  schema = {
    year: Joi.number().integer().allow("").min(1800).max(3000).label("Year"),
    genre: Joi.string().allow("").label("Genre"),
    top: Joi.number().integer().allow("").min(1).max(3000).label("Top Movies"),
  };

  columns = [
    {
      dataField: "movieId",
      text: "key",
      hidden: true,
    },
    {
      dataField: "title",
      text: "Movie Title",
      headerAlign: "center",
      headerClasses: "text-nowrap",
    },
    {
      dataField: "genres",
      text: "Genre",
      headerAlign: "center",
      headerClasses: "text-nowrap",
      classes: "text-nowrap",
      formatter: (cell, row) => cell.map((g) => g.genre).join(", ") || "-",
    },
    {
      dataField: "averageRating",
      text: "Average Rating",
      headerClasses: "text-nowrap",
      headerAlign: "center",
      align: "center",
      formatter: (cell, row) => (cell ? formatNumber(cell, 1, 1) : "-"),
    },
  ];

  async getMovies() {
    const { year, genre, top } = this.state.data;

    let queryMovies = { orderBy: "title asc", filter: {} };

    if (year) {
      queryMovies.filter = { ...queryMovies.filter, year: parseInt(year) };
    }
    if (top) {
      queryMovies = {
        ...queryMovies,
        top: parseInt(top),
        orderBy: "averageRating desc, title asc",
      };
    }

    const query = buildQuery(queryMovies);

    const { data: movies } = await moviesService.getMovies(query);
    this.setState({ movies });
  }

  async getGenres() {
    const { data: genres } = await genresService.getGenres();
    const genresOptions = genres.map((g) => ({ id: g, name: g }));
    this.setState({ genres: genresOptions });
  }

  async componentDidMount() {
    const movies = this.getMovies();
    const genres = this.getGenres();

    await movies;
    await genres;
    this.setState({ ready: true });
  }

  doSubmit = async () => {
    await this.getMovies();
    this.setState({ submitting: false });
  };

  state = {
    movies: [],
    genres: [],
    ready: false,
    submitting: false,
    data: { year: "", genre: "", top: "" },
    errors: {},
  };
  render() {
    const { movies, ready, genres } = this.state;
    return (
      <Row>
        <Col>
          <Card>
            <CardHeader className="bg-dark text-white h5">
              List of Movies
            </CardHeader>
            <CardBody>
              {ready ? (
                <React.Fragment>
                  <Form className="form-row" onSubmit={this.handleSubmit}>
                    <Col md={2}>
                      {this.renderInput("year", "Year", "number")}
                    </Col>
                    <Col md={2}>
                      {this.renderSelect("genre", genres, "Genre")}
                    </Col>
                    <Col md={2}>
                      {this.renderInput("top", "Top N Rated Movies", "number")}
                    </Col>
                    <FormGroup style={{ marginTop: 32 }}>
                      {this.renderButton("Apply filter")}
                    </FormGroup>
                  </Form>
                  <BootstrapTable
                    bootstrap4
                    keyField="movieId"
                    data={movies}
                    columns={this.columns}
                    striped={true}
                    hover={true}
                    classes={"table-responsive search-react-table"}
                    noDataIndication={"No Movies found"}
                  ></BootstrapTable>
                </React.Fragment>
              ) : (
                "Loading..."
              )}
            </CardBody>
          </Card>
        </Col>
      </Row>
    );
  }
}

export default Movies;
