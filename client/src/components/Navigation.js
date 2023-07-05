import React, { useState } from 'react';
import { Button, Container, Nav, Navbar } from 'react-bootstrap';
import Login from './Login';
import Register from './Register';

const appName = process.env.REACT_APP_APP_NAME

export const NonAuthNavigation = () => {
    const [showLoginModal, setShowLoginModal] = useState(false);
    const [showRegisterModal, setShowRegisterModal] = useState(false);

    return (
        <>
            <Navbar bg="light" expand="lg">
                <Container>
                    <span className="navbar-brand">{appName}</span>
                    <div className="ml-auto">
                        <Button variant="primary" onClick={() => setShowLoginModal(true)}>
                            Login
                        </Button>
                        <Button variant="secondary" onClick={() => setShowRegisterModal(true)}>
                            Register
                        </Button>
                    </div>
                </Container>
            </Navbar>
            <Login shouldShow={showLoginModal} onClose={() => setShowLoginModal(false)} />
            <Register shouldShow={showRegisterModal} onClose={() => setShowRegisterModal(false)} onRegister={() => { setShowRegisterModal(false);  setShowLoginModal(true); } } />
        </>
    )
}


export const AuthNavigation = () => (
    <Navbar bg="light" expand="lg">
        <Container>
            <span className="navbar-brand">{appName}</span>
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="ml-auto">
                    <Nav.Link href="/dashboard">Invoices</Nav.Link>
                </Nav>

            </Navbar.Collapse>

            <div className="ml-auto">
                <Button variant="secondary" className="ml-auto button button-secondary">
                    <Nav.Link href="/logout">Logout</Nav.Link>
                </Button>
            </div>
        </Container>

    </Navbar>
);