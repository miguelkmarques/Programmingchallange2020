import http from "./httpService";
import { apiUrl } from "../config.json";

const apiEndpoint = apiUrl + "/movies";

function getMovies(query = null) {
  return http.get(apiEndpoint + (query ? query : ""));
}

export default {
  getMovies,
};
