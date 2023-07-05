import React, { useEffect, useState } from 'react';
import apiService from '../appServices/apiService';
import CreateInvoice from './CreateInvoice';
import Invoices from './Invoices';
import InvoiceDetails from './InvoiceDetails';

const Dashboard = () => {
  const [invoices, setInvoices] = useState([]);

  useEffect(() => {
      fetchInvoices();
  }, []);

  const fetchInvoices = async () => {
    try {
      const response = await apiService.get('/invoices');
      setInvoices(response.data);
    } catch (error) {
      console.error('Error fetching invoices:', error);
    }
  };

  const handleCreateInvoice = (newInvoice) => {
    setInvoices([...invoices, newInvoice]);
  };

  return (
    <div>
      <div>
        <CreateInvoice onCreateInvoice={handleCreateInvoice} />
      </div>
      <div>
        <InvoiceDetails invoices={invoices}/>
      </div>
      <div>
        <Invoices invoices={invoices} />
      </div>
    </div>
  );
}

export default Dashboard;
