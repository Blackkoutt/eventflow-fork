import { useEffect, useState } from "react";
import { useFormContext } from "react-hook-form";

interface CheckboxProps {
  name: string;
  color: string;
  text: string;
  textColor: string;
  fontSize?: number;
  width?: number;
  height?: number;
  onCheck?: (isChecked: boolean, name?: string) => void;
}

const Checkbox = ({
  color,
  text,
  name,
  textColor,
  fontSize = 12,
  width = 16,
  height = 16,
}: CheckboxProps) => {
  const [isChecked, setIsChecked] = useState(false);
  const { register, getValues, watch } = useFormContext();

  useEffect(() => {
    setIsChecked(getValues(name));
  }, [watch()]);

  return (
    <div className="flex gap-2">
      <input
        {...register(name)}
        type="checkbox"
        id="some_id"
        style={{
          backgroundColor: isChecked ? `${color}` : "#FFFFFF",
          borderColor: `${color}`,
          width: `${width}px`,
          height: `${height}px`,
        }}
        className={`relative peer shrink-0
            appearance-none border-2 rounded-sm bg-white mt-1 hover:cursor-pointer`}
        //onChange={(e) => setIsChecked(e.target.checked)}
      />
      <label
        htmlFor="some_id"
        style={{ fontSize: `${fontSize}px`, color: textColor }}
        className="text-left flex items-center hover:cursor-pointer"
      >
        {text}
      </label>
      <svg
        className="
      absolute 
      w-4 h-4 mt-1 p-[2px]
      hidden peer-checked:block
      pointer-events-none"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        fill="none"
        stroke="white"
        strokeWidth="4"
        strokeLinecap="round"
        strokeLinejoin="round"
      >
        <polyline points="20 6 9 17 4 12"></polyline>
      </svg>
    </div>
  );
};
export default Checkbox;
