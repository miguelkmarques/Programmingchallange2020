import React, { Component } from "react";
import { Row, Col, Button, Card, CardHeader, CardBody } from "reactstrap";
import BootstrapTable from "react-bootstrap-table-next";
import formatNumber from "./../../helpers/formatNumber";

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
      classes: "text-nowrap",
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

  state = { data: [] };
  render() {
    const { data } = this.state;
    return (
      <Row>
        <Col>
          <Card>
            <CardHeader className="bg-secondary text-white">
              List of Movies
            </CardHeader>
            <CardBody>
              <BootstrapTable
                keyField="movieId"
                data={data}
                columns={this.columns}
              ></BootstrapTable>
            </CardBody>
          </Card>
        </Col>
      </Row>
    );
  }
}

export default Movies;
