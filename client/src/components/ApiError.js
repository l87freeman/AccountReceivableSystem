import React from 'react';

const ApiError = ({ error }) => {
  return (
    <div>
      {error && <div className="alert alert-danger" role="alert">
        {error}
      </div>}
    </div>
  );
};

export default ApiError;
