import React, { useState, useEffect, ChangeEvent } from 'react';

interface EmailAutocompleteProps {
  value: string;
  onChange: (value: string) => void;
  disabled?: boolean;
  placeholder?: string;
  name?: string;
  className?: string;
  required?: boolean;
}

/**
 * Email otomatik tamamlama bileşeni
 * Kullanıcı @ işaretinden sonra domain önerileri sunar
 */
const EmailAutocomplete: React.FC<EmailAutocompleteProps> = ({
  value,
  onChange,
  disabled = false,
  placeholder = "Email adresinizi girin",
  name = "email",
  className = "",
  required = false
}) => {
  // Ortak mail domainleri
  const [domains] = useState<string[]>([
    'mf.karaelmas.edu.tr',
    'beun.edu.tr',
    'gmail.com',
    'hotmail.com',
    'outlook.com',
    'yahoo.com',
    'icloud.com'
  ]);
  
  // Email giriş durumu
  const [emailInput, setEmailInput] = useState(value);
  const [domainSuggestions, setDomainSuggestions] = useState<string[]>([]);
  const [showSuggestions, setShowSuggestions] = useState(false);
  
  // Email değeri değiştiğinde input değerini güncelle
  useEffect(() => {
    setEmailInput(value);
  }, [value]);
  
  // Email input değiştiğinde domain önerilerini filtrele
  useEffect(() => {
    const atIndex = emailInput.indexOf('@');
    
    if (atIndex > 0) {
      const prefix = emailInput.substring(0, atIndex);
      const typedDomain = emailInput.substring(atIndex + 1).toLowerCase();
      
      // Domain önerilerini filtrele
      const filteredDomains = domains.filter(domain => 
        domain.toLowerCase().startsWith(typedDomain)
      );
      
      setDomainSuggestions(filteredDomains.map(domain => `${prefix}@${domain}`));
      setShowSuggestions(filteredDomains.length > 0);
    } else {
      setShowSuggestions(false);
    }
  }, [emailInput, domains]);
  
  // Input değişikliklerini yakala
  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setEmailInput(newValue);
    onChange(newValue);
  };
  
  // Domain önerisi seçildiğinde
  const handleSelectDomain = (email: string) => {
    setEmailInput(email);
    onChange(email);
    setShowSuggestions(false);
  };

  return (
    <div className="relative">
      <input 
        type="email"
        name={name}
        value={emailInput}
        onChange={handleChange}
        onBlur={() => setTimeout(() => setShowSuggestions(false), 200)}
        className={`w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60 ${className}`}
        placeholder={placeholder}
        disabled={disabled}
        required={required}
        autoComplete="email"
      />
      
      {/* Email Autocomplete Dropdown */}
      {showSuggestions && (
        <div className="absolute z-10 w-full mt-1 bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-auto">
          <ul className="py-1">
            {domainSuggestions.map((suggestion, index) => (
              <li 
                key={index} 
                className="px-4 py-2 text-gray-800 hover:bg-gray-100 cursor-pointer"
                onClick={() => handleSelectDomain(suggestion)}
              >
                {suggestion}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default EmailAutocomplete; 