import React, { useState } from 'react';
import { Card, Form, Button } from 'react-bootstrap';
import apiService from '../appServices/apiService';
import { formatDate } from '../helpers/dateUnils';

const Invoice = ({ invoice }) => {
  const [isPaid, setPaid] = useState(invoice.isPaid);

  const setInvoicePaid = async (invoice) => {
    try {
      await apiService.put(`/invoices/${invoice.id}`, { isPaid: true });
      invoice.isPaid = true;
      setPaid(true);
    } catch (error) {
      console.error('Error updating invoice as paid:', error);
    }
  };

  const formatItems = (lineItems) => {
    return lineItems.map(li => `Description: ${li.description}; Quantity: ${li.quantity}; TotalPrice: ${li.totalPrice};`).join("\n");
  }

  return (
    <Card>
      <Card.Body>
        <Form>
          <Form.Group controlId="id" hidden>
            <Form.Control type="text" value={invoice.id} readOnly />
          </Form.Group>

          <Form.Group controlId="customerName">
            <Form.Label>Customer Name</Form.Label>
            <Form.Control type="text" value={invoice.customerName} readOnly />
          </Form.Group>

          <Form.Group controlId="invoiceNumber">
            <Form.Label>Invoice Number</Form.Label>
            <Form.Control type="text" value={invoice.invoiceNumber} readOnly />
          </Form.Group>

          <Form.Group controlId="invoiceDate">
            <Form.Label>Invoice Date</Form.Label>
            <Form.Control type="date" value={formatDate(invoice.invoiceDate)} readOnly />
          </Form.Group>

          <Form.Group controlId="dueDate">
            <Form.Label>Due Date</Form.Label>
            <Form.Control type="date" value={formatDate(invoice.dueDate)} readOnly />
          </Form.Group>

          {invoice.payDate && (<Form.Group controlId="payDate">
            <Form.Label>Pay Date</Form.Label>
            <Form.Control type="date" value={formatDate(invoice.payDate)} readOnly />
          </Form.Group>)}

          <Form.Group controlId="lineItems">
            <Form.Label>Line Items</Form.Label>
            <Form.Control as="textarea" rows={3} value={formatItems(invoice.lineItems)} readOnly />
          </Form.Group>

          {!isPaid && (<Button variant="primary" type="submit" onClick={() => setInvoicePaid(invoice)}>
            Set Paid
          </Button>)}
        </Form>
      </Card.Body>
    </Card>
  );
};
export default Invoice;