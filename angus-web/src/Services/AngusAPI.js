let baseUrl = 'https://lazybuffalo-api.azurewebsites.net';

const postZone = (zone) => {
  return fetch(baseUrl + "/api/zone", {
    method: "POST",
    body: zone
  })
};

const getCows = () => {
  return fetch(baseUrl + "/api/cows/locations/fake")
};

export {
  postZone,
  getCows
}
