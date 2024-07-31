export function handleClassMutation(setIsModalOpen: React.Dispatch<React.SetStateAction<boolean | undefined>>): MutationCallback {
  return (mutationsList) => {
    mutationsList.forEach(mutation => {
      if (mutation.attributeName === 'class') {
        const isClassChanged = (mutation.target as Element).classList.contains('active');
        setIsModalOpen(isClassChanged);
      }
    });
  };
}