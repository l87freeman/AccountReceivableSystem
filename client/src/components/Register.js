import React, { useState } from 'react';
import { Button, Modal, Form } from 'react-bootstrap';
import authService from '../appServices/authService';
import dataFetcher from '../helpers/dataFetcher';
import ApiError from './ApiError';

const Register = ({ shouldShow, onClose, onRegister }) => {
    const [error, setError] = useState('');
    const [email, setEmail] = useState('');
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');

    const handleRegister = async () => {
        const result = await dataFetcher(async () => await authService.register({ userName, email, password }));
        if (result.error) {
            setError(result.error);
            return;
        }

        onRegister();
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
                <Modal.Title>Register and then Login</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group key="email" controlId="email">
                        <Form.Label>Email</Form.Label>
                        <Form.Control type="email" placeholder="Provide email" onChange={(e) => setEmail(e.target.value)} />
                    </Form.Group>
                    <Form.Group key="userName" controlId="userName">
                        <Form.Label>User Name</Form.Label>
                        <Form.Control type="email" placeholder="Provide User Name" onChange={(e) => setUserName(e.target.value)} />
                    </Form.Group>
                    <Form.Group key="password" controlId="password">
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Provide Password" onChange={(e) => setPassword(e.target.value)} />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>Close</Button>
                <Button variant="primary" type="submit" onClick={handleRegister}>Register</Button>
            </Modal.Footer>
        </Modal>
    );
};

export default Register;
