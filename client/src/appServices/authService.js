import axios from 'axios';

const identityUrl = process.env.REACT_APP_IDENTITY_URL;
const tokenString = 'token';

const authApiService = axios.create({
  baseURL: `${identityUrl}/auth`,
  headers: {
    'Content-Type': 'application/json',
  }
});

const getToken = () => {
  return localStorage.getItem(tokenString);
};

const isAuthenticated = () => {
  const token = getToken();
  return token !== null;
};

const login = async (loginModel) => {
  const response = await authApiService.post('/login', loginModel);
  localStorage.setItem(tokenString, response.data.access_token);
  return response;
};

const register = async (registerModel) => {
  const response = await authApiService.post('/register', registerModel);
  return response;
};

const logout = () => {
  localStorage.removeItem(tokenString);
};

const authService = { getToken, isAuthenticated, login, register, logout };

export default authService;
