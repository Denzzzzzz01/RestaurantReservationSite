import React, { useEffect } from "react";
import "./Modal.scss";

interface ModalProps {
  title: string;
  isOpen: boolean;
  onClose: () => void;
  children: React.ReactNode;
  className?: string;
}

const Modal: React.FC<ModalProps> = ({ title, isOpen, onClose, children, className }) => {
  useEffect(() => {
    const handleEsc = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        onClose();
      }
    };

    const handleClickOutside = (event: MouseEvent) => {
      if ((event.target as HTMLElement).classList.contains("modal-overlay")) {
        onClose();
      }
    };

    if (isOpen) {
      document.addEventListener("keydown", handleEsc);
      document.addEventListener("click", handleClickOutside);
    }

    return () => {
      document.removeEventListener("keydown", handleEsc);
      document.removeEventListener("click", handleClickOutside);
    };
  }, [isOpen, onClose]);

  return isOpen ? (
    <div className="modal">
      <div className="modal-overlay"></div>
      <div className={`modal-content ${className || ""}`}>
        <div className="modal-header">
          <h4>{title}</h4>
          <button className="modal-close" onClick={onClose}>
            ✖
          </button>
        </div>
        <div className="modal-body">{children}</div>
      </div>
    </div>
  ) : null;
};

export default Modal;
