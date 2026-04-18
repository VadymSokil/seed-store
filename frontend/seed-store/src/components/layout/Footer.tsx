import { useNavigate } from 'react-router-dom';

const Footer = () => {
  const navigate = useNavigate();
  
  return (
    <footer className="bg-green-700 text-white mt-auto">
      <div className="px-4 md:px-8 xl:px-8 2xl:px-32 py-6 border-b border-green-600">
        {/* Десктоп */}
        <div className="hidden md:flex justify-between items-start">
          <div className="flex flex-col gap-1">
            <span className="font-bold text-xl tracking-widest">SEED STORE</span>
          </div>
          <div className="flex gap-6 text-sm pt-1">
            <button onClick={() => navigate('/info#about')} className="hover:underline">Про нас</button>
            <button onClick={() => navigate('/info#delivery')} className="hover:underline">Оплата і доставка</button>
            <button onClick={() => navigate('/info#contacts')} className="hover:underline">Контакти</button>
          </div>
          <div className="flex flex-col items-end gap-2">
            <span className="text-sm">Підтримка: <a href="https://t.me/your_bot" className="hover:underline">Telegram-бот</a></span>
            
          </div>
        </div>

        {/* Мобільна */}
        <div className="md:hidden flex flex-col items-center gap-4 text-sm">
          <div className="flex flex-col items-center gap-1">
            <span className="font-bold text-xl tracking-widest">SEED STORE</span>
          </div>
          <div className="flex flex-col items-center gap-2">
            <button onClick={() => navigate('/info#about')} className="hover:underline">Про нас</button>
            <button onClick={() => navigate('/info#delivery')} className="hover:underline">Оплата і доставка</button>
            <button onClick={() => navigate('/info#contacts')} className="hover:underline">Контакти</button>
          </div>
          <div className="flex flex-col items-center gap-2">
            <span>Підтримка: <a href="https://t.me/your_bot" className="hover:underline">Telegram-бот</a></span>
          </div>
        </div>
      </div>
      <div className="px-4 md:px-8 xl:px-8 2xl:px-32 py-4 text-sm text-center">
        © {new Date().getFullYear()} Seed Store. Всі права захищені.
      </div>
    </footer>
  );
}

export default Footer;