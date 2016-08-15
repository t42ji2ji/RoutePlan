using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Vertex
{
    private string _ID;
    private float _longitude, _latitude;

    public Vertex(string ID, float longitude, float latitude)
    {
        _ID = ID;
        _longitude = longitude;
        _latitude = latitude;
    }

    public string ID
    {
        get{ return _ID;}
        set{ _ID = value;}
    }

    public float Longitude
    {
        get { return _longitude; }
        set { _longitude = value; }
    }

    public float Latitude
    {
        get { return _latitude; }
        set { _latitude = value; }
    }

    public override string ToString()
    {
        return string.Format("[{0}]({1:0.00},{2:0.00})", _ID, _longitude, _latitude);
    }
}

