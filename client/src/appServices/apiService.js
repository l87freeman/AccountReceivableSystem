import axios from 'axios';
import authService from './authService'

const apiUrl = process.env.REACT_APP_API_URL;

const apiService = axios.create({
  baseURL: `${apiUrl}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiService.interceptors.request.use((config) => {
  const token = authService.getToken();
 
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  } else {
    throw new Error('Token is empty');
  }

  return config;
});

export default apiService;
