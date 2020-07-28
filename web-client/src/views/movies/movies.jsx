import React from "react";
import { Row, Col, Card, CardHeader, CardBody } from "reactstrap";
import BootstrapTable from "react-bootstrap-table-next";
import buildQuery from "odata-query";
import Joi from "joi-browser";
import formatNumber from "./../../helpers/formatNumber";
import moviesService from "./../../services/moviesService";
import genresService from "./../../services/genresService";
import DefaultForm from "./../../components/defaultForm";

class Movies extends DefaultForm {
  schema = {
    year: Joi.number().integer().min(1800).max(3000).label("Year"),
    genre: Joi.string().label("Genre"),
    top: Joi.number().integer().min(1).max(3000).label("Top Movies"),
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
      formatter: (cell, row) => cell.join(", ") || "-",
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
    let queryMovies = { orderBy: "averageRating desc, title asc" };
    const query = buildQuery(queryMovies);

    const { data: movies } = await moviesService.getMovies(query);
    this.setState({ movies });
  }

  async getGenres() {
    const { data: genres } = await genresService.getGenres();
    this.setState({ genres });
  }

  async componentDidMount() {
    const movies = this.getMovies();
    const genres = this.getGenres();

    await movies;
    await genres;
    this.setState({ ready: true });
  }

  state = {
    movies: [],
    genres: [],
    ready: false,
    data: { year: "", genre: "", top: "" },
  };
  render() {
    const { movies: data, ready } = this.state;
    return (
      <Row>
        <Col>
          <Card>
            <CardHeader className="bg-dark text-white h5">
              List of Movies
            </CardHeader>
            <CardBody>
              {ready ? (
                <BootstrapTable
                  bootstrap4
                  keyField="movieId"
                  data={data}
                  columns={this.columns}
                  striped={true}
                  hover={true}
                  classes={"table-responsive search-react-table"}
                  noDataIndication={"No Movies found"}
                ></BootstrapTable>
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
