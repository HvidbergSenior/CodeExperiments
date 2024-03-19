import { useEffect, useState } from "react";

export function useDebounce<T>(value: T, delay?: number) {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);
  const [isDebouncing, setIsDebouncing] = useState(false);

  useEffect(() => {
    setIsDebouncing(true);
    const timer = setTimeout(() => {
      setIsDebouncing(false);
      setDebouncedValue(value);
    }, delay || 500);

    return () => {
      clearTimeout(timer);
    };
  }, [value, delay]);

  return { debouncedValue, isDebouncing };
}
