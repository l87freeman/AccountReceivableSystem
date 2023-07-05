import React, { useState } from 'react';
import { Form, Button, Modal, Container } from "react-bootstrap";
import apiService from '../appServices/apiService';
import { formatDate } from '../helpers/dateUnils';
import ApiError from './ApiError';
import fetchData from '../helpers/dataFetcher';

const CreateInvoice = ({ onCreateInvoice }) => {
  const [showModal, setShowModal] = useState(false);
  const [customerName, setCustomerName] = useState("");
  const [invoiceNumber, setInvoiceNumber] = useState("");
  const [invoiceDate, setInvoiceDate] = useState("");
  const [dueDate, setDueDate] = useState("");
  const [lineItems, setLineItems] = useState([]);
  const [error, setError] = useState('');
  const [validated, setValidated] = useState(false);

  const handleAddLineItem = () => {
    setLineItems([...lineItems, { description: "", quantity: 0, totalPrice: 0 }]);
  };

  const handleLineItemChange = (index, field, value) => {
    const updatedLineItems = [...lineItems];
    updatedLineItems[index][field] = value;
    setLineItems(updatedLineItems);
  };

  const onClose = () => {
    setError('');
    setCustomerName("");
    setInvoiceNumber("");
    setInvoiceDate("");
    setDueDate("");
    setLineItems([]);
    setShowModal(false);
    setValidated(false);
  };

  const allowSubmit = () => {
    let isValid = customerName && invoiceNumber && invoiceDate && dueDate;
    if(lineItems && lineItems.length > 0){
      isValid = lineItems.reduce((isValid, curr) => isValid = isValid && curr.description && curr.quantity > 0 && curr.totalPrice > 0, isValid);
    }    

    return isValid;
  }

  const handleFormSubmit = async (event) => {
    event.preventDefault();
    if(!allowSubmit()){
       setValidated(true);
       return;
    }

    setValidated(false);
    const newInvoice = await fetchData(async () => await apiService.post('/invoices', {
      customerName,
      invoiceNumber,
      invoiceDate,
      dueDate,
      lineItems,
    }));

    if (newInvoice.error) {
      setError(newInvoice.error);
      return;
    }

    onCreateInvoice(newInvoice.response.data);

    onClose();
  };

  return (
    <>
      <Container>
        <Button variant="primary" onClick={() => setShowModal(true)}>Create Invoice</Button>
      </Container>

      <Modal show={showModal}>
        <Modal.Header>
          <Modal.Title>Create Invoice</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <ApiError error={error} />
          <Form noValidate validated={validated}>
            <Form.Group controlId="customerName">
              <Form.Label>Customer Name</Form.Label>
              <Form.Control
                type="text"
                value={customerName}
                required
                onChange={(e) => setCustomerName(e.target.value)}
              />
              <Form.Control.Feedback type="invalid">
                Value must not be empty.
              </Form.Control.Feedback>
            </Form.Group>

            <Form.Group controlId="invoiceNumber">
              <Form.Label>Invoice Number</Form.Label>
              <Form.Control
                type="text"
                value={invoiceNumber}
                required
                onChange={(e) => setInvoiceNumber(e.target.value)}
              />
              <Form.Control.Feedback type="invalid">
                Value must not be empty.
              </Form.Control.Feedback>
            </Form.Group>

            <Form.Group controlId="invoiceDate">
              <Form.Label>Invoice Date</Form.Label>
              <Form.Control
                type="date"
                value={formatDate(invoiceDate)}
                required
                onChange={(e) => setInvoiceDate(e.target.value)}
              />
              <Form.Control.Feedback type="invalid">
                Value must not be empty.
              </Form.Control.Feedback>
            </Form.Group>

            <Form.Group controlId="dueDate">
              <Form.Label>Due Date</Form.Label>
              <Form.Control
                type="date"
                value={formatDate(dueDate)}
                required
                onChange={(e) => setDueDate(e.target.value)}
              />
              <Form.Control.Feedback type="invalid">
                Value must not be empty.
              </Form.Control.Feedback>
            </Form.Group>

            <Form.Group controlId="lineItems">
              <Form.Label>Line Items</Form.Label>
              {lineItems.map((lineItem, index) => (
                <div key={index}>
                  <Form.Control
                    type="text"
                    placeholder="Description"
                    value={lineItem.description}
                    required
                    onChange={(e) => handleLineItemChange(index, "description", e.target.value)}
                  />
                  <Form.Control
                    type="number"
                    placeholder="Quantity"
                    value={lineItem.quantity}
                    required
                    min={1}
                    onChange={(e) => handleLineItemChange(index, "quantity", parseInt(e.target.value))}
                  />
                  <Form.Control
                    type="number"
                    placeholder="Total Price"
                    value={lineItem.totalPrice}
                    required
                    min={1}
                    onChange={(e) => handleLineItemChange(index, "totalPrice", parseFloat(e.target.value))}
                  />
                  <Form.Control.Feedback type="invalid">
                    Description must not be empty, Quantity and Total Price must be greater than 0.
                  </Form.Control.Feedback>
                </div>
              ))}
              <Button variant="secondary" onClick={handleAddLineItem}>Add Line Item</Button>
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={onClose}>Close</Button>
          <Button variant="primary" type="submit" form="invoiceForm" onClick={handleFormSubmit}>Create Invoice</Button>
        </Modal.Footer>
      </Modal>
    </>
  );
};

export default CreateInvoice;
