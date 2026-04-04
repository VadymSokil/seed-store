import { useEffect } from 'react';

export const useModalScrollLock = (isOpen?: boolean, scrollableRef?: React.RefObject<HTMLElement | null>) => {
  useEffect(() => {
    if (!isOpen) return;
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
  }, [isOpen]);
};