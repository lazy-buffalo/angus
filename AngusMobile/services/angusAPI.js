const baseUrl = 'https://lazybuffalo-api.azurewebsites.net';

const debug = false;

const postZone = (zone) => {
  return fetch(baseUrl + "/api/zone", {
    method: "POST",
    body: zone
  })
};

const getCows = (start, end) => {
  return fetch(`${baseUrl}/api/cows/${debug ? "fake/20/20" : ""}?start=${start}&end=${end}`)
};

const getGrazing = () => {
  return fetch(baseUrl + "/api/grazing")
};

export {
  postZone,
  getCows,
  getGrazing
}
