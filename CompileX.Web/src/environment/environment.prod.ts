// export const environment = {
//   production: true,
//   apiBaseUrl: 'https://realestate-web-api.onrender.com/'
// };
export const environment = {
  production: true,
  apiBaseUrl: (window as any)['NG_APP_API_BASE_URL']
};
