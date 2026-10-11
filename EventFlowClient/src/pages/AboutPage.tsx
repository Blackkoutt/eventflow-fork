import target from "../assets/target.png";
import tickets3d from "../assets/tickets3d.png";
import concert from "../assets/concert.jpg";
import hero from "../assets/hero_section_img.png";

function AboutPage() {
  return (
    <div className="py-10 w-[80%] max-w-[1400px]">
      <div className="flex flex-col gap-6 mt-6">
        <article className="w-full flex flex-col justify-center items-center">
          <h2 className="tracking-wide">
            <span className="text-[#2F2F2F] text-4xl font-semibold">Witaj w </span>
            <span className="text-primaryPurple font-extrabold text-5xl">EventFlow!</span>
          </h2>
          <p className="max-w-[80%] text-center mt-2 text-textPrimary">
            Szukasz przestrzeni, gdzie sztuka i pasja splatają się w jedno? To właśnie EventFlow! To
            tutaj dźwięki, obrazy i historie łączą ludzi, tworząc niezapomniane chwile. Niezależnie
            od tego, czy kochasz teatr, muzykę czy film – u nas odnajdziesz magię wyjątkowych
            wydarzeń.
          </p>
        </article>
        <div className="flex flex-col justify-center items-center gap-6 mt-12">
          <div className="flex flex-row justify-between items-center">
            <article className="max-w-[55%] -translate-y-6 flex flex-col gap-2">
              <h3 className="font-bold text-[26px]">Nasza historia</h3>
              <p className="text-textPrimary text-justify">
                EventFlow powstało w 2024 roku z pasji do kultury i sztuki, jako odpowiedź na
                rosnącą potrzebę łączenia artystów, organizatorów i miłośników wyjątkowych wydarzeń.
                Początkowo skupialiśmy się na organizacji wydarzeń i festiwali, współpracując z
                twórcami i instytucjami kulturalnymi, aby dostarczać publiczności niezapomniane
                przeżycia. Jednak z biegiem czasu zauważyliśmy, że potrzeby zarówno organizatorów,
                jak i uczestników wykraczają poza same wydarzenia. W odpowiedzi na to rozwinęliśmy
                naszą działalność i uruchomiliśmy nowoczesną platformę online do sprzedaży biletów,
                która stała się intuicyjnym i wygodnym narzędziem dla każdego, kto chce uczestniczyć
                w kulturze. Dziś EventFlow to nie tylko platforma biletowa, ale ekosystem
                wspierający kulturę i sztukę na wielu poziomach. Łączymy ludzi, miejsca i emocje,
                tworząc przestrzeń, gdzie każda chwila staje się wyjątkowym doświadczeniem.
              </p>
            </article>
            <img
              src={hero}
              className="object-contain max-w-[40%] max-h-[350px]"
              alt="Zdjęcie koncertu"
            ></img>
          </div>
          <div className="flex flex-row justify-between items-center">
            <img
              src={target}
              className="object-contain max-w-[40%] max-h-[320px]"
              alt="Zdjęcie celu"
            />
            <article className="max-w-[55%] -translate-y-6 flex flex-col gap-2">
              <h3 className="font-bold text-[26px]">Nasz cel</h3>
              <p className="text-textPrimary text-justify">
                Naszą misją jest nie tylko ułatwianie dostępu do wydarzeń, tak by każdy mógł znaleźć
                coś dla siebie. Oferujemy zarówno wielkie festiwale i głośne premiery, jak i
                kameralne koncerty, spektakle czy niszowe inicjatywy artystyczne. Niezależnie od
                tego, czy szukasz energetycznych występów światowej klasy artystów, inspirujących
                przedstawień teatralnych, czy klimatycznych wydarzeń lokalnych – u nas znajdziesz
                wszystko w jednym miejscu. Chcemy, aby każda osoba mogła znaleźć wydarzenie, które
                trafi prosto w jej gust i pozwoli przeżyć wyjątkowe chwile. EventFlow to Twój
                przewodnik po świecie wydarzeń kulturalnych!
              </p>
            </article>
          </div>
          <div className="flex flex-row justify-between items-center">
            <article className="max-w-[55%] -translate-y-6 flex flex-col gap-2">
              <h3 className="font-bold text-[26px]">Nasza oferta</h3>
              <p className="text-textPrimary text-justify">
                Na EventFlow znajdziesz bilety na koncerty, spektakle teatralne, wystawy oraz wiele
                innych wyjątkowych wydarzeń kulturalnych w tym także festiwali. Dbamy o różnorodność
                oferty, aby każdy niezależnie od gustu czy zainteresowań mógł znaleźć coś dla
                siebie. Dzięki karnetom możesz uczestniczyć w ulubionych wydarzeniach bez
                konieczności kupowania oddzielnych biletów, oszczędzając czas i pieniądze. Może
                planujesz własne wydarzenie? Nic prostszego! Oferujemy wynajem sal – od kameralnych
                przestrzeni po duże sceny, idealne na koncerty, spektakle czy konferencje. Odkrywaj,
                twórz i przeżywaj wyjątkowe chwile razem z nami!
              </p>
            </article>
            <img
              src={tickets3d}
              className="object-contain max-w-[40%] max-h-[320px]"
              alt="Zdjęcie biletów"
            ></img>
          </div>
        </div>
      </div>
    </div>
  );
}
export default AboutPage;
