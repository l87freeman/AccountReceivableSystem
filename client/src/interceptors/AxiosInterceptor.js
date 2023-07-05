import { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import apiService from '../appServices/apiService';
import authService from '../appServices/authService';

const AxiosInterceptor = ({ children }) => {

    const navigate = useNavigate();

    useEffect(() => {

        const resInterceptor = response => {
            return response;
        }

        const errInterceptor = error => {
            if (error.response?.status === 401) {
                authService.logout();
                navigate('/');
            }

            return Promise.reject(error);
        }


        const interceptor = apiService.interceptors.response.use(resInterceptor, errInterceptor);

        return () => apiService.interceptors.response.eject(interceptor);

    }, [navigate])

    return children;
}

export default AxiosInterceptor;