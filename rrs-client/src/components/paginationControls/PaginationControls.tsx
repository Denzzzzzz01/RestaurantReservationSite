import React from "react";
import './PaginationControls.scss';

interface PaginationControlsProps {
  pageNumber: number;
  totalPages: number;
  onNext: () => void;
  onPrevious: () => void;
  onPageSelect: (page: number) => void;
}

const PaginationControls: React.FC<PaginationControlsProps> = ({
  pageNumber,
  totalPages,
  onNext,
  onPrevious,
  onPageSelect,
}) => {
  const renderPageButtons = () => {
    const pageButtons = [];
    const delta = 2;

    for (let i = 1; i <= totalPages; i++) {
      if (
        i === 1 ||
        i === totalPages ||
        (i >= pageNumber - delta && i <= pageNumber + delta) 
      ) {
        pageButtons.push(
          <button
            key={i}
            className={`pagination-button ${
              i === pageNumber ? "active" : ""
            }`}
            onClick={() => onPageSelect(i)}
            disabled={i === pageNumber} 
          >
            {i}
          </button>
        );
      } else if (
        i === pageNumber - delta - 1 || 
        i === pageNumber + delta + 1 
      ) {
        pageButtons.push(
          <span key={`ellipsis-${i}`} className="pagination-ellipsis">
            ...
          </span>
        );
      }
    }

    return pageButtons;
  };

  return (
    <div className="pagination-controls">
      <button
        onClick={onPrevious}
        disabled={pageNumber === 1}
        className="pagination-nav"
      >
        Previous
      </button>
      {renderPageButtons()}
      <button
        onClick={onNext}
        disabled={pageNumber >= totalPages}
        className="pagination-nav"
      >
        Next
      </button>
    </div>
  );
};

export default PaginationControls;
