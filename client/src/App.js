import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Dashboard from './components/Dashboard';
import NotFound from './components/NotFound';
import AuthRoute from './components/AuthRoute';
import Logout from './components/Logout';
import { AuthNavigation, NonAuthNavigation } from './components/Navigation';
import { useAuth } from './providers/AuthProvider';
import AxiosInterceptor from './interceptors/AxiosInterceptor';

const App = () => {
  const { isAuthenticated } = useAuth();

  return (
    <BrowserRouter>
      <AxiosInterceptor>
        {isAuthenticated ? <AuthNavigation /> : <NonAuthNavigation />}

        <Routes>
          <Route index element={<></>} />
          <Route path="logout" element={<Logout />} />
          <Route
            path="dashboard"
            element={<AuthRoute isAuthenticated={isAuthenticated} > <Dashboard /> </AuthRoute>}
          />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </AxiosInterceptor >

    </BrowserRouter>
  );
};

export default App;