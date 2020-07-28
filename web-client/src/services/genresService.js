import http from "./httpService";
import { apiUrl } from "../config.json";

const apiEndpoint = apiUrl + "/genres";

function getGenres() {
  return http.get(apiEndpoint);
}

export default {
  getGenres,
};
