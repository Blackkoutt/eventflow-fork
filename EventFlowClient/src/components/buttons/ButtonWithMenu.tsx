import { IconDefinition } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";

export type ButtonWithMenuElement = {
  icon: IconDefinition;
  text: string;
  iconColor: string;
  action: () => void;
};

interface ButtonWithMenuProps {
  text: string;
  width: number;
  height: number;
  bgColor?: string;
  textColor?: string;
  textSize?: number;
  icon: IconDefinition;
  iconSize?: number;
  menuElements: ButtonWithMenuElement[];
}

const ButtonWithMenu = ({
  text,
  bgColor = "#7B2CBF",
  textColor = "#fff",
  textSize = 14,
  icon,
  iconSize = 15,
  menuElements,
}: ButtonWithMenuProps) => {
  const [menuVisibility, setMenuVisibility] = useState(false);

  return (
    <button
      className="relative px-4 py-3 rounded-lg"
      style={{ backgroundColor: bgColor }}
      onFocus={() => setMenuVisibility(true)}
      onBlur={() => setMenuVisibility(false)}
    >
      <div className="flex flex-row justify-center items-center gap-3">
        <FontAwesomeIcon icon={icon} fontSize={iconSize} color={textColor}></FontAwesomeIcon>
        <span style={{ fontSize: textSize, color: textColor }}>{text}</span>
      </div>
      <div
        className={`${
          menuVisibility ? "flex" : "hidden"
        } absolute top-full left-0 flex-col justify-start w-full items-center rounded-lg bg-white shadow-lg text-textPrimary`}
      >
        {menuElements.map((element, index) => (
          <div
            key={index}
            className="flex flex-row p-3 items-center justify-start rounded-sm gap-2 w-full hover:bg-[#efefef]"
            onClick={() => element.action()}
          >
            <FontAwesomeIcon
              icon={element.icon}
              style={{ color: `${element.iconColor ? element.iconColor : "#fff"}` }}
            ></FontAwesomeIcon>
            <span style={{ fontSize: textSize }}>{element.text}</span>
          </div>
        ))}
      </div>
    </button>
  );
};
export default ButtonWithMenu;
