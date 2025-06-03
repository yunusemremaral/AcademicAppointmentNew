import React from 'react';
import { Akademisyen } from '../../services/Ogrenci/departmentService';
import AkademisyenLinkItem from './AkademisyenLinkItem';

// Akademisyen kartı bileşeni - AkademisyenLinkItem'ı kullanır
const AkademisyenKart: React.FC<{ akademisyen: Akademisyen }> = ({ akademisyen }) => {
  return (
    <AkademisyenLinkItem 
      akademisyen={akademisyen}
      displayType="card"
    />
  );
};

export default AkademisyenKart;
