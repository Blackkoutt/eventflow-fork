import Button, { ButtonStyle } from "../components/buttons/Button";
import hero from "../assets/hero_section_img.png";
import SectionHeader from "../components/common/SectionHeader";
import NewsList from "../components/homepage/NewsList";
import EventList from "../components/homepage/EventList";
import FestivalList from "../components/homepage/FestivalList";
import FaqList from "../components/homepage/FaqList";
import PartnerList from "../components/homepage/PartnerList";
import EventPassCTA from "../components/homepage/EventPassCTA";

const HomePage = () => {
  return (
    <>
      <section
        id="hero"
        className="py-[82px] hero_section  flex flex-col lg:flex-row items-center justify-center gap-6 px-4"
      >
        <div className="flex flex-col justify-center items-start gap-7 max-w-lg lg:max-w-none">
          <article className="flex flex-col justify-start items-start gap-5 max-w-[1200px]">
            <h1>Witaj w EventFlow!</h1>
            <p className="text-[16px] font-normal text-textPrimary">
              Zajmujemy się organizacją różnorodnych wydarzeń kulturalnych, takich jak pokazy
              teatralne, koncerty i festiwale. Nasze przestrzenie są dostępne na wynajem z opcją
              dodatkowych usług, co sprawia, że organizacja Twojego wydarzenia będzie prosta i
              bezproblemowa. Oferujemy bilety oraz karnety, które zapewniają wstęp na każde nasze
              wydarzenie. Nie zwlekaj - zanurz się w wir fascynujących wydarzeń z EventFlow!
            </p>
          </article>
          <Button
            text="Sprawdź wydarzenia"
            width={220}
            height={60}
            fontSize={16}
            style={ButtonStyle.Primary}
            action={() => {}}
          />
        </div>
        <img
          src={hero}
          alt="Obrazek powitalny EventFlow"
          className="w-full max-w-[688px] lg:max-w-[50%] h-auto"
        />
      </section>

      <section
        id="events"
        className="min-h-[100px] min-w-[125%] overflow-auto bg-[#F4F6FA] flex flex-col justify-center items-center"
      >
        <div className="w-[80%] flex flex-col justify-center items-center py-16 gap-12">
          <SectionHeader
            title="Nadchodzące wydarzenia"
            subtitle="W tej sekcji znajdziesz zbliżające się wydarzenia z różnych kategorii: koncert,
              spektakl, film itd."
          />
          <div className="flex flex-col justify-center items-center gap-6">
            <FestivalList />
            <EventList />
          </div>
        </div>
      </section>
      <section id="news" className="pt-16">
        <div className="flex flex-col justify-center items-center gap-12">
          <SectionHeader
            title="Aktualności"
            subtitle="W tej sekcji znajdziesz najnowsze informacje, ogłoszenia i relacje z wydarzeń"
          />
          <NewsList />
        </div>
      </section>
      <section id="cta">
        <EventPassCTA />
      </section>
      <section
        id="faq"
        className="min-h-[100px] min-w-[125%] overflow-auto bg-[#F4F6FA] flex flex-col justify-center items-center"
      >
        <div className="mt-48 w-[80%]">
          <div className="flex flex-col justify-center items-center gap-7">
            <SectionHeader
              title="FAQ"
              subtitle="Znajdź odpowiedzi na najbardziej nurtujące cię pytania"
            />
            <FaqList faqCountToDisplay={4} />
          </div>
        </div>
      </section>
      <section
        id="partners"
        className="min-w-[125%] bg-[#F4F6FA] flex flex-col justify-center items-center"
      >
        <div className="mt-24 w-[80%]">
          <div className="relative flex flex-col justify-center items-center gap-9">
            <SectionHeader title="Partnerzy" subtitle="Sprawdź z kim współpracujemy" />
            <PartnerList />
          </div>
        </div>
      </section>
    </>
  );
};
export default HomePage;
