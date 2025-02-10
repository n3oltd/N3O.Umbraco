export const transformCrop = crop => {
    if (!crop) {
        return {}
    }

    const transformedCrop = {
        bottomLeft: {
            x: crop.x,
            y: crop.y
        },
        topRight: {
            x: crop.x + crop.width,
            y: crop.y + crop.height
        }
    }

    return transformedCrop;
}