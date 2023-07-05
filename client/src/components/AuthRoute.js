import React from 'react';
import { Navigate } from 'react-router-dom';
import authService from '../appServices/authService';

const AuthRoute = ({ isAuthenticated, children }) => {
  if (!isAuthenticated && !authService.isAuthenticated()) {
    return <Navigate to="/" replace />;
  }

  return children;
};

export default AuthRoute;