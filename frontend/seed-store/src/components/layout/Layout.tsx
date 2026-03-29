import { useRef, useEffect, useState } from 'react';
import Footer from './Footer';
import HeaderTop from './HeaderTop';
import HeaderLower from './HeaderLower';

const Layout = ({ children }: { children: React.ReactNode }) => {
  const headerRef = useRef<HTMLDivElement>(null);
  const [headerHeight, setHeaderHeight] = useState(0);

  useEffect(() => {
    if (headerRef.current) {
      setHeaderHeight(headerRef.current.offsetHeight);
    }
  }, []);

  return (
    <div className="min-h-screen flex flex-col w-full">
      <div className="hidden md:block">
        <HeaderTop />
      </div>
      <div className="fixed md:sticky top-0 z-40 shadow-sm w-full" ref={headerRef}>
        <HeaderLower />
      </div>
      <div className="md:hidden" style={{ height: headerHeight }} />
      <main className="flex-1 px-4 md:px-8 xl:px-8 2xl:px-32">
        {children}
      </main>
      <Footer />
    </div>
  );
}

export default Layout;