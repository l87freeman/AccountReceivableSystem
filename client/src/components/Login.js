import React, { useState } from 'react';
import authService from '../appServices/authService';
import dataFetcher from '../helpers/dataFetcher';
import { useAuth } from '../providers/AuthProvider';
import { Button, Modal, Form } from 'react-bootstrap';
import ApiError from './ApiError';

const Login = ({shouldShow, onClose}) => {
    const [error, setError] = useState('');
    const { setIsAuthenticated } = useAuth();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = async () => {
        const result = await dataFetcher(async () => await authService.login({ email, password }));

        if (result.error) {
            setError(result.error);
            return;
        }
        
        setIsAuthenticated(true);
        handleClose();
    };

    const handleClose = () => {
        setError('');
        onClose();
    }

    return (
        <Modal show={shouldShow}>
            <ApiError error={error} />
            <Modal.Header>
                <Modal.Title>Login</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group key="email" controlId="email">
                        <Form.Label>Email</Form.Label>
                        <Form.Control type="email" placeholder="Provide email" onChange={(e) => setEmail(e.target.value)} />
                    </Form.Group>
                    <Form.Group key="password" controlId="password">
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Provide Password" onChange={(e) => setPassword(e.target.value)} />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>Close</Button>
                <Button variant="primary" type="submit" onClick={handleLogin}>Login</Button>
            </Modal.Footer>
        </Modal>
    );
};

export default Login;
