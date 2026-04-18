import { useNavigate } from 'react-router-dom';

const HeaderTop = () => {
  const navigate = useNavigate();

  return (
    <div className="bg-green-700 text-white text-sm py-2">
      <div className="px-4 md:px-8 xl:px-8 2xl:px-32 flex justify-between items-center">
        <span className="font-bold text-xl tracking-widest md:text-left text-center w-full md:w-auto">SEED STORE</span>
        <div className="hidden md:flex gap-6">
          <button onClick={() => navigate('/info#about')} className="hover:underline">Про нас</button>
          <button onClick={() => navigate('/info#delivery')} className="hover:underline">Оплата і доставка</button>
          <button onClick={() => navigate('/info#contacts')} className="hover:underline">Контакти</button>
        </div>
      </div>
    </div>
  );
}

export default HeaderTop;