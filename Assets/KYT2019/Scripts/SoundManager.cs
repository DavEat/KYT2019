using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioClip m_selectionSound = null;
    [SerializeField] AudioClip m_unselectionSound = null;
    [SerializeField] AudioClip m_sendSound = null;
    [SerializeField] AudioClip m_upgradeSound = null;
    [SerializeField] AudioClip m_buildSound = null;
    [SerializeField] AudioClip m_moveSound = null;
    [SerializeField] AudioClip m_errorSound = null;
    [SerializeField] AudioClip m_buttonSound = null;
    [SerializeField] AudioClip m_closeSound = null;

    AudioSource m_source;
    void Start()
    {
        m_source = GetComponent<AudioSource>();
    }

    public void PlaySelection()
    {
        m_source.PlayOneShot(m_selectionSound);
    }
    public void PlayUnselection()
    {
        m_source.PlayOneShot(m_unselectionSound);
    }
    public void PlaySend()
    {
        m_source.PlayOneShot(m_sendSound);
    }
    public void PlayUpgrade()
    {
        m_source.PlayOneShot(m_upgradeSound);
    }
    public void PlayBuild()
    {
        m_source.PlayOneShot(m_buildSound);
    }
    public void PlayMove()
    {
        m_source.PlayOneShot(m_moveSound);
    }
    public void PlayError()
    {
        m_source.PlayOneShot(m_errorSound);
    }
    public void PlayButton()
    {
        m_source.PlayOneShot(m_buttonSound);
    }
    public void PlayClose()
    {
        m_source.PlayOneShot(m_closeSound);
    }
}
