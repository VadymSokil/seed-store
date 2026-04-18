import { useEffect, useState } from 'react';

export const useCountdown = (seconds: number, active: boolean) => {
  const [timeLeft, setTimeLeft] = useState(seconds);

  useEffect(() => {
    if (!active) return;
    setTimeLeft(seconds);
    const interval = setInterval(() => {
      setTimeLeft(prev => {
        if (prev <= 1) { clearInterval(interval); return 0; }
        return prev - 1;
      });
    }, 1000);
    return () => clearInterval(interval);
  }, [active, seconds]);

  const minutes = String(Math.floor(timeLeft / 60)).padStart(2, '0');
  const secs = String(timeLeft % 60).padStart(2, '0');

  return { formatted: `${minutes}:${secs}`, expired: timeLeft === 0 };
};