import React, { Component } from "react";
import { Row, Col, Button, Card, CardHeader, CardBody } from "reactstrap";
import BootstrapTable from "react-bootstrap-table-next";
import buildQuery from "odata-query";
import formatNumber from "./../../helpers/formatNumber";
import moviesService from "./../../services/moviesService";

class Movies extends Component {
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

  async componentDidMount() {
    await this.getMovies();
    this.setState({ ready: true });
  }

  state = { movies: [], ready: false };
  render() {
    const { movies: data } = this.state;
    return (
      <Row>
        <Col>
          <Card>
            <CardHeader className="bg-secondary text-white">
              List of Movies
            </CardHeader>
            <CardBody>
              <BootstrapTable
                bootstrap4
                keyField="movieId"
                data={data}
                columns={this.columns}
                hover={true}
                classes={"table-responsive search-react-table"}
                noDataIndication={"No Movies found"}
              ></BootstrapTable>
            </CardBody>
          </Card>
        </Col>
      </Row>
    );
  }
}

export default Movies;
