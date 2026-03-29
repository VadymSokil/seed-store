const HeaderTop = () => {
  return (
    <div className="bg-green-700 text-white text-sm py-2">
      <div className="px-4 md:px-8 xl:px-8 2xl:px-32 flex justify-between items-center">
        <span className="font-bold text-xl tracking-widest md:text-left text-center w-full md:w-auto">SEED STORE</span>
        <div className="hidden md:flex gap-6">
          <a href="#" className="hover:underline">Про нас</a>
          <a href="#" className="hover:underline">Оплата і доставка</a>
          <a href="#" className="hover:underline">Контакти</a>
        </div>
        <span className="hidden md:block">Графік роботи: Пн-Пт 9:00–18:00</span>
      </div>
    </div>
  );
}

export default HeaderTop;