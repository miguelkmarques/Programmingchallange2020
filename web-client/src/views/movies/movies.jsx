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
  //Schema Joi do formulário de Pesquisa para impedir que o usuário use parâmetros inválidos
  schema = {
    year: Joi.number().integer().allow("").min(1800).max(3000).label("Year"),
    genre: Joi.string().allow("").label("Genre"),
    top: Joi.number().integer().allow("").min(1).max(3000).label("Top Movies"),
  };

  //Colunas que devem aparecer ou não aparecer na tela e aplicando formatters customizados
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
      //Função para concatenar a lista de Genres do Movie, separado por vírgula
      formatter: (cell, row) => cell.join(", ") || "-",
    },
    {
      dataField: "averageRating",
      text: "Average Rating",
      headerClasses: "text-nowrap",
      headerAlign: "center",
      align: "center",
      //Função para sempre mostrar uma casa depois da vírgula
      formatter: (cell, row) => (cell ? formatNumber(cell, 1, 1) : "0.0"),
    },
  ];

  //Função para recuperar a lista de Movies da API Rest, aplicando a query OData
  //de acordo com o filtro do usuário
  async getMovies() {
    const { year, genre, top } = this.state.data;

    //por padrão aplica a ordenação por Title em ordem ascendente
    let queryMovies = { orderBy: "title asc", filter: {} };

    //se o year tiver um valor, adiciona na propriedade filter
    if (year) {
      queryMovies.filter = { ...queryMovies.filter, year: parseInt(year) };
    }
    //se o top tiver um valor, adiciona na propriedade top
    if (top) {
      queryMovies = {
        ...queryMovies,
        top: parseInt(top),
        orderBy: "averageRating desc, title asc",
      };
    }

    //aplica a função buildQuery para retornar a string equivalente a query
    let query = buildQuery(queryMovies);

    //se o genre tiver um valor, concatena na string query no final
    if (genre) {
      query = `${query}&genre=${genre}`;
    }

    //usa o moviesService para realizar a requisição HTTP GET para a API
    const { data: movies } = await moviesService.getMovies(query);
    //atualiza o State com a lista de Movies
    this.setState({ movies });
  }

  async getGenres() {
    //usa o genresService para realizar a requisição HTTP GET para a API
    const { data: genres } = await genresService.getGenres();
    const genresOptions = genres.map((g) => ({ id: g, name: g }));
    //atualiza o State com a lista de Genres
    this.setState({ genres: genresOptions });
  }

  //No momento que a página carrega, chama as funções para recuperar a lista de Movies e
  //a lista de Genres que serve para aplicar o filtro de Genre no formulário
  async componentDidMount() {
    const movies = this.getMovies();
    const genres = this.getGenres();

    await movies;
    await genres;
    this.setState({ ready: true });
  }

  //Após o usuário clicar no botão e ser validado os campos do formulário, chama a função getMovies
  //para atualizar a lista de Movies de acordo com o filtro aplicado pelo usuário
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
