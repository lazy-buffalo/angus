const baseUrl = 'https://lazybuffalo-api.azurewebsites.net';

let debug = true;
if (window.location.href.indexOf('localhost') > 0) {
  debug = true;
}

const getCows = (start, end) => {
  return fetch(`${baseUrl}/api/cows/${debug ? "fake/20/10000" : ""}?start=${start}&end=${end}`)
};

const deleteLocations = () => {
  return fetch(`${baseUrl}/api/cows/delete/locations`, {
    method: "POST"
  })
};

const getGrazing = () => {
  return fetch(`${baseUrl}/api/grazing`);
};

const postGrazing = (grazing) => {

  const headers= new Headers({
    "Content-Type": "application/json",
  });

  var request = new Request(`${baseUrl}/api/grazing`, {
    method : 'POST',
    headers: new Headers({
      'Content-Type': 'application/json'
    }),
    body : JSON.stringify(grazing)
  });

  return fetch(request);
};

export {
  getCows,
  deleteLocations,
  postGrazing,
  getGrazing
}
