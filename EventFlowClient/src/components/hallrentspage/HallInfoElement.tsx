import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ReactNode } from "react";

interface HallInfoElementProps {
  text: string;
  icon: IconProp;
  value: ReactNode;
}

const HallInfoElement = ({ text, icon, value }: HallInfoElementProps) => {
  return (
    <div className="flex flex-col justify-center items-center gap-2">
      <p className="text-primaryPurple font-bold text-[26px]">{value}</p>
      <div className="flex flex-row gap-2">
        <FontAwesomeIcon className="text-primaryPurple text-[17px]" icon={icon} />
        <p className="font-semibold text-[17px] text-textPrimary">{text}</p>
      </div>
    </div>
  );
};
export default HallInfoElement;
