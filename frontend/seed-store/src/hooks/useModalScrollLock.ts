import { useEffect } from 'react';

export const useModalScrollLock = (scrollableRef?: React.RefObject<HTMLElement | null>) => {
  useEffect(() => {
    const preventDefault = (e: Event) => {
      if (scrollableRef?.current && scrollableRef.current.contains(e.target as Node)) return;
      e.preventDefault();
    };
    document.addEventListener('wheel', preventDefault, { passive: false });
    document.addEventListener('touchmove', preventDefault, { passive: false });
    return () => {
      document.removeEventListener('wheel', preventDefault);
      document.removeEventListener('touchmove', preventDefault);
    };
  }, []);
};