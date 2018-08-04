const baseUrl = 'https://lazybuffalo-api.azurewebsites.net';

let debug = false;
if (window.location.href.indexOf('localhost') > 0) {
  debug = false;
}

const postZone = (zone) => {
  return fetch(baseUrl + "/api/zone", {
    method: "POST",
    body: zone
  })
};

const getCows = (start, end) => {
  return fetch(`${baseUrl}/api/cows/${debug ? "fake/20/20" : ""}?start=${start}&end=${end}`)
};

const deleteLocations = () => {
  return fetch(`${baseUrl}/api/delete/locations`, {
    method: "POST"
  })
};

export {
  postZone,
  getCows,
  deleteLocations
}
