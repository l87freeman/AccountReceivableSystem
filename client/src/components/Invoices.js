import React, { useState } from 'react';
import { Form, Container } from "react-bootstrap";
import Invoice from "./Invoice";

const Invoices = ({ invoices }) => {
  const [unpaidOnly, setUnpaidOnly] = useState(false);

  const filteredInvoices = unpaidOnly ? invoices.filter((inv) => !inv.isPaid) : invoices;

  return (
    <>
      {invoices.length ? (
        <Container>
          <h1 className="text-primary">Invoices</h1>
          <Form>
            <Form.Check
              type="checkbox"
              id="checkbox-example"
              label="Only unpaid"
              checked={unpaidOnly}
              onChange={() => setUnpaidOnly(!unpaidOnly)}
            />
          </Form>
          {filteredInvoices.map((invoice) => <Invoice key={invoice.id} invoice={invoice} />)}
        </Container>
      ) : (
        <p className="text-muted">No invoices found.</p>
      )}      
    </>
  );
};

export default Invoices;