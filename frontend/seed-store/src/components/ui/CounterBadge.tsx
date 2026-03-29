interface CounterBadgeProps {
  count: number;
}

const CounterBadge = ({ count }: CounterBadgeProps) => {
  if (count === 0) return null;
  
  return (
    <span className="absolute -top-1.5 -right-1.5 bg-green-600 text-white text-[10px] font-bold rounded-full min-w-[18px] h-[18px] flex items-center justify-center px-1">
      {count > 99 ? '99+' : count}
    </span>
  );
}

export default CounterBadge;