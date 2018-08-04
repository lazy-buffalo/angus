const baseUrl = 'https://lazybuffalo-api.azurewebsites.net';

let debug = false;
if (window.location.href.indexOf('localhost')) {
  debug = true;
}

const postZone = (zone) => {
  return fetch(baseUrl + "/api/zone", {
    method: "POST",
    body: zone
  })
};

const getCows = () => {
  return fetch(`${baseUrl}/api/cows/locations/${debug ? "fake/20/20" : ""}`)
};

const deleteLocations = () => {
  return fetch(baseUrl + "/api/location/delete", {
    method: "POST"
  })
};

export {
  postZone,
  getCows,
  deleteLocations
}
