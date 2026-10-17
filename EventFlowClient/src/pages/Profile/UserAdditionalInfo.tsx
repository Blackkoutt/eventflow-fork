import { useOutletContext } from "react-router-dom";
import { User } from "../../models/response_models";
import ProfileInfo from "../../components/profile/ProfileInfo";
import Button, { ButtonStyle } from "../../components/buttons/Button";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";
import { useRef } from "react";
import ChangeAdditionalInfoDialog from "../../components/profile/ChangeAdditionalInfoDialog";

interface OutletContextProps {
  user: User;
  reloadAdditonalInfoComponent: () => void;
}

const UserAdditionalInfo = () => {
  const { user, reloadAdditonalInfoComponent } = useOutletContext<OutletContextProps>();
  const dialogRef = useRef<HTMLDialogElement | null>(null);

  const reloadComponent = () => {
    dialogRef.current?.close();
    reloadAdditonalInfoComponent();
  };

  return (
    <article className="flex flex-col justify-start items-start p-4">
      <h3 className="font-bold text-[27px] pb-4">Dodatkowe informacje o użytkowniku:</h3>
      <div className="flex flex-col justify-start items-start gap-4">
        <ProfileInfo infoTitle="Ulica:" infoContent={user.userData?.street} />
        <ProfileInfo infoTitle="Nr domu:" infoContent={`${user.userData?.houseNumber}`} />
        <ProfileInfo infoTitle="Nr mieszkania:" infoContent={`${user.userData?.flatNumber}`} />
        <ProfileInfo infoTitle="Miasto:" infoContent={user.userData?.city} />
        <ProfileInfo infoTitle="Kod pocztowy:" infoContent={user.userData?.zipCode} />
        <ProfileInfo infoTitle="Nr telefonu:" infoContent={user.userData?.phoneNumber} />
      </div>
      <ChangeAdditionalInfoDialog ref={dialogRef} user={user} reloadComponent={reloadComponent} />
      <div className="mt-5">
        <Button
          text="Edytuj"
          width={130}
          height={50}
          rounded="rounded-full"
          iconSize={20}
          fontSize={16}
          isFontSemibold={true}
          style={ButtonStyle.Primary}
          icon={faPenToSquare}
          action={() => dialogRef.current?.showModal()}
        />
      </div>
    </article>
  );
};
export default UserAdditionalInfo;
