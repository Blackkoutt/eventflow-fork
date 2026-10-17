import { User } from "../../models/response_models";
import ApiClient from "../../services/api/ApiClientService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-solid-svg-icons";
import { useState } from "react";

interface ProfilePhotoProps {
  user: User;
  cacheBuster: number;
  onClick: () => void;
}

const ProfilePhoto = ({ user, cacheBuster, onClick }: ProfilePhotoProps) => {
  const [hover, setHover] = useState(false);

  return (
    <div
      className="relative bg-slate-400 shadow-lg rounded-full w-[190px] h-[190px] overflow-hidden hover:cursor-pointer hover:opacity-95"
      onMouseEnter={() => setHover(true)}
      onClick={onClick}
      onMouseLeave={() => setHover(false)}
    >
      <FontAwesomeIcon
        icon={faPenToSquare}
        className="absolute"
        style={{
          color: "white",
          top: "calc(50% - 20px)",
          left: "calc(50% - 20px)",
          width: "40px",
          height: "40px",
          display: `${hover ? "block" : "none"}`,
        }}
      />
      <img
        src={`${ApiClient.GetPhotoEndpoint(user.photoEndpoint)}?t=${cacheBuster}`}
        alt={`Zdjęcie użytkownika ${user.name} ${user.surname}`}
        className="w-full h-full object-cover"
      />
    </div>
  );
};
export default ProfilePhoto;
