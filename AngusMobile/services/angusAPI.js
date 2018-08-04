const baseUrl = 'https://lazybuffalo-api.azurewebsites.net';

const postZone = (zone) => {
  return fetch(baseUrl + "/api/zone", {
    method: "POST",
    body: zone
  })
};

const getCows = () => {
  return fetch(baseUrl + "/api/cows/fake/10/10")
};

export {
  postZone,
  getCows
}
