import React from 'react';
import { Student } from '../../../lib/api';

interface OgrenciKartProps {
  student: Pick<Student, 'id' | 'userFullName' | 'email'>;
}

/**
 * Öğrenci bilgilerini gösteren kart bileşeni
 * @param props.student Öğrenci bilgileri
 */
const OgrenciKart: React.FC<OgrenciKartProps> = ({ student }) => {
  return (
    <div className="bg-white shadow-md rounded-lg p-4 hover:shadow-lg transition-shadow duration-300 border border-gray-200">
      <div className="flex flex-col">
        {/* Öğrenci Adı */}
        <h3 className="font-semibold text-lg text-gray-800 mb-1">{student.userFullName}</h3>
        
        {/* Öğrenci Email */}
        <p className="text-sm text-gray-600 flex items-center">
          <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
          </svg>
          {student.email}
        </p>
      </div>
    </div>
  );
};

export default OgrenciKart;
