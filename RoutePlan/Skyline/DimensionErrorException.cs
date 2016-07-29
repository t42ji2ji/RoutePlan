using System;


class DimensionErrorException : Exception
{
    public DimensionErrorException(int correctDimension, int errorDimension)
        : base("Dimension is " + errorDimension + ", but correct dimension is " + correctDimension) { }
}

