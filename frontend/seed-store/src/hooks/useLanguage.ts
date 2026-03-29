import { useState } from 'react';

export type Language = 'UA' | 'EN';

export const useLanguage = () => {
  const [language, setLanguage] = useState<Language>('UA');

  const toggleLanguage = (lang: Language) => {
    setLanguage(lang);
  };

  return { language, toggleLanguage };
};