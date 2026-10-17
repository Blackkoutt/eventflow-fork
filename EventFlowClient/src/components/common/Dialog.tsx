import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { forwardRef, useRef } from "react";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";

interface DialogProps {
  children?: React.ReactNode;
  onClose?: () => void;
  maxWidth?: number;
  paddingLeft?: number;
  paddingRight?: number;
  minWidth?: number;
  minHeight?: number;
  maxHeight?: number;
  marginTop?: number;
  top?: number;
}

const Dialog = forwardRef<HTMLDialogElement, DialogProps>(
  (
    {
      children,
      minHeight,
      minWidth,
      paddingLeft = 28,
      paddingRight = 28,
      maxWidth,
      marginTop,
      onClose,
      top,
    }: DialogProps,
    ref
  ) => {
    const dialogRef = useRef<HTMLDialogElement | null>(null);

    return (
      <dialog
        ref={(node) => {
          dialogRef.current = node;
          if (typeof ref === "function") {
            ref(node);
          } else if (ref) {
            ref.current = node;
          }
        }}
        style={{
          minHeight: minHeight,
          zIndex: 99,
          minWidth: minWidth,
          paddingLeft: paddingLeft,
          paddingRight: paddingRight,
          marginTop: marginTop,
          maxWidth: maxWidth,
          position: "absolute",
          top: top ? window.scrollY + top : undefined,
        }}
        className="rounded-xl py-7 relative overflow-visible max-h-none"
      >
        <FontAwesomeIcon
          icon={faCircleXmark}
          className="absolute -right-3 -top-3 hover:cursor-pointer hover:opacity-95 bg-white rounded-full"
          style={{
            width: "35px",
            height: "35px",
          }}
          onClick={() => {
            dialogRef.current?.close();
            onClose?.();
          }}
        />
        {children}
      </dialog>
    );
  }
);
export default Dialog;
