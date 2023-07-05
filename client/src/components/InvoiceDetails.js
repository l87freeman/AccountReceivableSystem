import React from 'react';
import { Container, Col, Row } from "react-bootstrap";
import { dateDiffSeconds } from '../helpers/dateUnils';

const InvoiceDetails = ({ invoices }) => {
    invoices = invoices || [];
    const unpaidAmount = invoices.reduce((s, c) => s += c.isPaid ? 0 : c.lineItems.reduce((ss, cl) => ss += cl.totalPrice, 0), 0);
    const avarageTime = invoices.reduce((s, c) => {
        s.time += (c.isPaid ? dateDiffSeconds(c.invoiceDate, c.payDate) : 0);
        s.cnt += (c.isPaid ? 1 : 0);
        return s;
    }, { time: 0, cnt: 0 });

    const calcAvarageDays = (avarageObj) => {
        if (!avarageObj.cnt) {
            return 0;
        }

        return Math.round(avarageObj.time / avarageObj.cnt / 60 / 60 / 24);
    }

    return (
        <>
            {invoices.length > 0 && (<Container>
                <Row>
                    <Col>
                        <p>Unpaid amount:</p>
                        <p>{unpaidAmount}</p>
                    </Col>
                    <Col>
                        <p>Average days for invoice to be paid:</p>
                        <p>{calcAvarageDays(avarageTime)}</p>
                    </Col>
                </Row>
            </Container>)}
        </>
    );
};

export default InvoiceDetails;